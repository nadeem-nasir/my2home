import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs/Subject';
@Component({
  selector: 'reset-password-confirmation',
  templateUrl: './reset-password-confirmation.component.html'
})
export class RsetPasswordConfirmationComponent implements OnInit, OnDestroy
{
  
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  constructor(public router: Router, public route: ActivatedRoute) { }

  public ngOnInit()
  {
    
  }

  public ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
