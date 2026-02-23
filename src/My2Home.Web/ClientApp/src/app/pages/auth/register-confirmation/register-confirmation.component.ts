import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'Nb-register-confirmation',
  templateUrl: './register-confirmation.component.html'
})
export class NbRegisterConfirmationComponent implements OnInit, OnDestroy {
  public emailConfirmed: boolean;
  public sub: any;

  constructor(public router: Router, public route: ActivatedRoute) { }

  public ngOnInit() {
    this.sub = this.router
      .routerState.root
      .queryParams
      .subscribe((params: any) => {
        this.emailConfirmed = (params.emailConfirmed && params.emailConfirmed.toLowerCase() === 'true');
      });

  }

  public ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
