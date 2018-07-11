import { Component, OnInit, Input } from '@angular/core';
import { DeviceService } from '../services/device.service';
import { PagingDto, ServiceResponse, BaseDto, ResultType } from '../code/dto';
import { ActivatedRoute } from '@angular/router';
import { Location, getLocaleDateTimeFormat } from '@angular/common';
import { LogService } from '../services/log.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-model-list',
  templateUrl: './model-list.component.html'
})
export class ModelListComponent implements OnInit {

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
      this.deviceService.postModel(this.entryDto).subscribe(serviceResponse => {
        this.entryDto.Id = serviceResponse.Data;
        this.dtoList.push(this.entryDto);
        this.resetEntry();
      });
    } else {
      this.deviceService.putModel(this.entryDto).subscribe(serviceResponse => {
        const i = this.dtoList.findIndex((obj => obj.Id === this.entryDto.Id));
        this.dtoList[i] = this.entryDto;
        this.resetEntry();
      });
    }
  }

  get(dto: BaseDto): void {
    this.deviceService.getModel(dto.Id).subscribe(serviceResponse => {
      this.entryDto = Object.assign({}, serviceResponse.Data);
    });
  }

  delete(dto: BaseDto): void {
    this.deviceService.deleteModel(dto.Id).subscribe(serviceResponse => {
      this.dtoList = this.dtoList.filter(h => h !== dto);
    });
  }

  list(): void {
    this.deviceService.listModel(this.searchDto, this.pagingDto).subscribe(
      serviceResponse => {
        this.dtoList = serviceResponse.Data;
      }
    );
  }

  // #endregion CRUD

  isValid(obj) {
    if (obj.BrandName == null || obj.ModelName == null) {
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

  open() {
    const modalRef = this.modalService.open(NgbdModalContentComponent);
    modalRef.componentInstance.name = 'World';
  }

}


@Component({
  selector: 'app-ngbd-modal-content-component',
  template: `
    <div class="modal-header">
      <h4 class="modal-title">Sample Modal</h4>
      <button type="button" class="close" aria-label="Close" (click)="activeModal.dismiss('Cross click')">
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
    <div class="modal-body">
      <p>Hello, {{name}}!</p>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-outline-dark" (click)="activeModal.close('Close click')">Close</button>
    </div>
  `
})
export class NgbdModalContentComponent {
  @Input() name;
  constructor(public activeModal: NgbActiveModal) { }
}
