import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService'
import { LoadingButton } from '../Button';

export class Housings extends Component {
  static displayName = Housings.name;

  constructor(props) {
    super(props);
    this.state = { housings: [], loading: true };
  }

  componentDidMount() {
    this.getHousingsData();
  }

  static renderHousingsTable(housings) {
      return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Short name</th>
            <th>Address</th>
            <th>Floors count</th>
          </tr>
        </thead>
          <tbody>
                {housings.map((housing) => {
                    return <tr key={housing.id}>
                            <td>{housing.name}</td>
                            <td>{housing.shortName}</td>
                            <td>{housing.address}</td>
                        <td>{housing.floorsCount}</td>
                            <td><LoadingButton/></td>
                        </tr>;
                    }
                )}
          </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Housings.renderHousingsTable(this.state.housings);

    return (
      <div>
        <h1 id="tabelLabel" >Housings</h1>
        <p>This component demonstrates housings.</p>
        {contents}
      </div>
    );
  }

  async getHousingsData() {
    //const token = await authService.getAccessToken();
      const response = await fetch('/api/housings/list');//, {
      //headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    //});
      const data = await response.json();
    this.setState({ housings: data, loading: false });
  }
}
