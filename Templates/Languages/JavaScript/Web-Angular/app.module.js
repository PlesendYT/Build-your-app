// AngularJS module — defines the main application module and components
// Contains a counter component and a greeting component

const app = angular.module('myApp', []);

/** Counter component — increment, decrement, and reset a numeric value */
app.component('appCounter', {
  template: `
    <section>
      <h2>Counter</h2>
      <p class="count">{{ $ctrl.count }}</p>
      <button ng-click="$ctrl.increment()">+1</button>
      <button ng-click="$ctrl.decrement()">-1</button>
      <button ng-click="$ctrl.reset()">Reset</button>
    </section>
  `,
  controller() {
    this.count = 0;
    this.increment = () => this.count++;
    this.decrement = () => this.count--;
    this.reset = () => { this.count = 0; };
  },
});

/** Greeting component — interactive greeting with two-way binding */
app.component('appGreeting', {
  template: `
    <section>
      <h2>Begrüßung / Greeting</h2>
      <input ng-model="$ctrl.name" placeholder="Dein Name / Your name" />
      <p>Hallo / Hello, <strong>{{ $ctrl.name || '—' }}</strong>!</p>
    </section>
  `,
  controller() {
    this.name = 'World';
  },
});
