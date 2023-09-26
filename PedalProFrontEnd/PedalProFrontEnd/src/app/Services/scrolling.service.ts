import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ScrollingService {
  
  private targetSource = new BehaviorSubject<string>('default');
  currentTarget = this.targetSource.asObservable();

  constructor() { }

  changeTarget(target: string) {
    this.targetSource.next(target);
  }
}
