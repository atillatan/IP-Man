import { Component, OnInit } from '@angular/core';
import { DeviceService } from '../services/device.service';
import { PagingDto } from '../code/dto';


@Component({
  selector: 'app-property',
  templateUrl: './property.component.html'
})
export class PropertyComponent implements OnInit {

  dtoList: any = [];
  entryDto: any = {};
  searchDto: any = {};

  constructor(private deviceService: DeviceService) {
  }

  ngOnInit() {
    this.list();
  }

  //#region CRUD Operations

  postOrPut(): void {
    if (!this.isValid(this.entryDto)) { return; }

    if (this.entryDto.Id == null) {
      this.deviceService.postProperty(this.entryDto).subscribe(serviceResponse => {
        this.entryDto.Id = serviceResponse.Data;
        this.dtoList.push(this.entryDto);
        this.resetEntry();
      });
    } else {
      this.deviceService.putProperty(this.entryDto).subscribe(serviceResponse => {
        const i = this.dtoList.findIndex((obj => obj.Id === this.entryDto.Id));
        this.dtoList[i] = this.entryDto;
        this.resetEntry();
      });
    }
  }

  get(dto: any): void {
    this.deviceService.getProperty(dto.Id).subscribe(serviceResponse => {
      this.entryDto = Object.assign({}, serviceResponse.Data);
    });
  }

  delete(dto: any): void {
    this.deviceService.deleteProperty(dto.Id).subscribe(serviceResponse => {
      this.dtoList = this.dtoList.filter(h => h !== dto);
    });
  }

  list(): void {
    this.deviceService.listProperty({}, new PagingDto()).subscribe(
      serviceResponse => {
        this.dtoList = serviceResponse.Data;
      }
    );
  }

  // #endregion CRUD

  isValid(obj) {
    if (obj.Name == null) {
      alert('Lutfen zorunlu alanlari doldurunuz!');
      return false;
    } else {
      return true;
    }
  }

  resetEntry() {
    this.entryDto = {};
    this.entryDto.UpdatedBy = 0;
    this.entryDto.UpdateDate = new Date();
  }


}


