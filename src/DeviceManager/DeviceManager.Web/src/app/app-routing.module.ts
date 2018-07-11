import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ModelListComponent } from './model/model-list.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { PropertyComponent } from './property/property.component';
import { TemplateComponent } from './template/template.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { AuthGuard } from './services/auth.guard';
import { LanguageComponent } from './language/language.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent},
  { path: 'model', component: ModelListComponent },
  { path: 'login', component: LoginComponent },
  { path: 'property', component: PropertyComponent },
  { path: 'template', component: TemplateComponent },
  { path: 'language', component: LanguageComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
