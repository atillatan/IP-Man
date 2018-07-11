import { Component, OnInit, Input } from '@angular/core';
import { DeviceService } from '../services/device.service';
import { PagingDto, ServiceResponse, BaseDto, ResultType } from '../code/dto';
import { ActivatedRoute } from '@angular/router';
import { Location, getLocaleDateTimeFormat } from '@angular/common';
import { LogService } from '../services/log.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-template',
  templateUrl: './template.component.html'
})
export class TemplateComponent implements OnInit {

  private url = 'http://localhost:5001/api/device'; // config servis ten gelecek.

  dtoList: any = [];
  entryDto: any = {};
  searchDto: BaseDto = new BaseDto();
  pagingDto: PagingDto = new PagingDto();

  constructor(
    private logService: LogService,
    private deviceService: DeviceService,
    private route: ActivatedRoute,
    private location: Location,
    private modalService: NgbModal
  ) {
    this.list();
  }

  ngOnInit() {

  }

  //#region CRUD Operations

  postOrPut(): void {
    if (!this.isValid(this.entryDto)) { return; }
    if (this.entryDto.Id == null) {
      this.deviceService.post(this.entryDto, `${this.url}/posttemplate`).subscribe(serviceResponse => {
        this.entryDto.Id = serviceResponse.Data;
        this.dtoList.push(this.entryDto);
        this.resetEntry();
      });
    } else {
      this.deviceService.put(this.entryDto, `${this.url}/puttemplate`).subscribe(serviceResponse => {
        const i = this.dtoList.findIndex((obj => obj.Id === this.entryDto.Id));
        this.dtoList[i] = this.entryDto;
        this.resetEntry();
      });
    }
  }

  get(dto: BaseDto): void {
    this.deviceService.get(dto.Id, `${this.url}/gettemplate`).subscribe(serviceResponse => {
      this.entryDto = Object.assign({}, serviceResponse.Data);
    });
  }

  delete(dto: BaseDto): void {
    this.deviceService.delete(dto.Id, `${this.url}/deletetemplate`).subscribe(serviceResponse => {
      this.dtoList = this.dtoList.filter(h => h !== dto);
    });
  }

  list(): void {
    this.deviceService.list(this.searchDto, this.pagingDto, `${this.url}/listtemplate`).subscribe(
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
    this.entryDto = new BaseDto();
    this.entryDto.UpdatedBy = 0;
    this.entryDto.UpdateDate = new Date();
  }

  goBack(): void {
    this.location.back();
  }



}


