import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserModule } from '@angular/platform-browser';
import { LayoutModule } from './layout/layout.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideHotToastConfig } from '@ngxpert/hot-toast';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AuthInterceptor } from './core/interceptor/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    LayoutModule,
  ],
  bootstrap: [AppComponent],
  providers: [
    provideHotToastConfig(),
    {provide:HTTP_INTERCEPTORS,useClass:AuthInterceptor,multi:true}
  ]
})

export class AppModule {
  constructor() {
    console.log('Anguler UI Module is Live....');
  }
}
