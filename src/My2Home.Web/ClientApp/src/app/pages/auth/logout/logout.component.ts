import { Component, Inject, OnInit, OnDestroy} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, } from '../../../services'
import { Subject } from 'rxjs/Subject';
@Component({
  selector: 'nb-logout',
  templateUrl: './logout.component.html',
})
export class NbLogoutComponent implements OnInit, OnDestroy {

  private ngUnsubscribe: Subject<void> = new Subject<void>();

  constructor(private router: Router, private route: ActivatedRoute,
    private authenticationService: AuthenticationService) {

  }
  ngOnInit(): void
  {
    this.authenticationService.logout();
    this.router.navigate(['../login'], { relativeTo: this.route });
  }

  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
