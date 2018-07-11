import { Component, OnInit } from '@angular/core';
import { PagingDto, ServiceResponse, BaseDto, ResultType } from '../code/dto';
import { Location, getLocaleDateTimeFormat } from '@angular/common';
import { PageEvent, MatDialog, MatDialogRef, MAT_DIALOG_DATA, Sort } from '@angular/material';
import { CrudService } from '../services/crud.service';
import { DeleteConfirmationComponent } from '../delete-confirmation/delete-confirmation.component';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css']
})
export class LanguageComponent implements OnInit {

  dtoList: any = [];
  entryDto: any = {};
  displayedColumns: any = [];
  searchDto: BaseDto = new BaseDto();
  pagingDto: PagingDto = new PagingDto();
  panelOpenState: Boolean = false;
  private url = 'http://localhost:5001/api/system'; // config servis ten gelecek.
  
  constructor(
    private crudService: CrudService,
    private location: Location,
    public dialog: MatDialog) {
      this.displayedColumns = ['Key', 'Val', 'LanguageCode'];
  }

  ngOnInit() {
    this.list();
  }

  //#region CRUD Operations

  postOrPut(): void {
    if (!this.isValid(this.entryDto)) { return; }

    if (this.entryDto.Id == null) {
      this.entryDto.Iseditable = 1;
      this.crudService.post(this.entryDto, `${this.url}/postlanguage`).subscribe(serviceResponse => {
        this.entryDto.Id = serviceResponse.Data;
        this.dtoList.push(this.entryDto);
        this.resetEntry();
      });
    } else {
      this.crudService.put(this.entryDto, `${this.url}/putlanguage`).subscribe(serviceResponse => {
        const i = this.dtoList.findIndex((obj => obj.Id === this.entryDto.Id));
        this.dtoList[i] = this.entryDto;
        this.resetEntry();
      });
    }
  }

  get(dto: any): void {
    this.crudService.get(dto.Id, `${this.url}/getlanguage`).subscribe(serviceResponse => {
      this.entryDto = Object.assign({}, serviceResponse.Data);
      this.panelOpenState = true;
    });
  }

  delete(dto: any): void {
    this.crudService.delete(dto.Id, `${this.url}/deletelanguage`).subscribe(serviceResponse => {
      this.dtoList = this.dtoList.filter(h => h !== dto);
    });
  }

  list(): void {
    this.crudService.list(this.searchDto, this.pagingDto, `${this.url}/listlanguage`).subscribe(
      serviceResponse => {
        this.dtoList = serviceResponse.Data;
        this.pagingDto.count = serviceResponse.TotalCount;
      }
    );
  }

  // #endregion CRUD

  isValid(obj) {
    if (obj.Key == null) {
      alert('Lutfen zorunlu alanlari doldurunuz!');
      return false;
    } else if (obj.Val == null) {
      alert('Lutfen zorunlu alanlari doldurunuz!');
      return false;
    } else if (obj.LanguageCode == null) {
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

  openDialog(dto: any): void {
    const dialogRef = this.dialog.open(DeleteConfirmationComponent, {
      width: '250px',
      data: { dto: dto }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.confirmation === 'YES') {
        this.delete(result.dto);
      }
      console.log('The dialog was closed');
    });
  }

  changePage(pageEvent: PageEvent): void {
    this.pagingDto.count = pageEvent.length;
    this.pagingDto.pageSize = pageEvent.pageSize;
    this.pagingDto.pageNumber = pageEvent.pageIndex + 1;
    this.list();
  }


  sortData(sort: Sort): void {
    if (!sort.active || sort.direction === '') {
      return;
    }
    // local paging
    this.dtoList = this.dtoList.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'Key': return this.compare(a.Key, b.Key, isAsc);
        case 'Value': return this.compare(a.Value, b.Value, isAsc);
        case 'LanguageCode': return this.compare(a.LanguageCode, b.LanguageCode, isAsc);
        default: return 0;
      }
    });

    // Database paging
    // this.pagingDto.order  = sort.direction;
    // this.list();
  }

  compare(a, b, isAsc) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }

}
