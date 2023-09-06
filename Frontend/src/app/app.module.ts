import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';

import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { SharedModule } from './shared/shared.module';
import { JwtInterceptorService } from './services/jwt-interceptor.service';
import { GreenbayModule } from './greenbay/greenbay.module';

@NgModule({
  declarations: [AppComponent],
  imports: [AppRoutingModule, SharedModule, GreenbayModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptorService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
