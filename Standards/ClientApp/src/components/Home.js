import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1>Hello, this is Standards!</h1>
        <p>It's used to catalogization of standards, using in metrological laboratory with next data:</p>
        <ul>
          <li>Dates of verification and calibration</li>
          <li>Metrological characteristics of standards</li>
          <li>Service maintenance and persons, who should do it</li>
          <li>Notifications about next verification, calibration and service maintenance</li>
          <li>Different information about standards: department, sector, housing and room, where standard is using and so on</li>
        </ul>
        <h2>Service apps</h2>
        <ul>
          <li><a href='https://localhost:3000/swagger'>Swagger</a></li>
        </ul>
      </div>
    );
  }
}
