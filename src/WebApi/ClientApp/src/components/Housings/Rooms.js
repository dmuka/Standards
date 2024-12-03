import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService'

export class Rooms extends Component {
    static displayName = Rooms.name;

  constructor(props) {
    super(props);
      this.state = { rooms: [], loading: true };
  }

  componentDidMount() {
    this.getRoomsData();
  }

    static renderHousingsTable(rooms) {
      return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Housing id</th>
            <th>Floor</th>
            <th>Length</th>
            <th>Height</th>
            <th>Width</th>
            <th>Sector id</th>
            <th>Comments</th>
          </tr>
        </thead>
          <tbody>
                  {rooms.map((room) => {
                      return <tr key={room.id}>
                          <td>{room.name}</td>
                          <td>{room.housingId}</td>
                          <td>{room.floor}</td>
                          <td>{room.length}</td>
                          <td>{room.height}</td>
                          <td>{room.width}</td>
                          <td>{room.sectorId}</td>
                          <td>{room.comments}</td>
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
      : Rooms.renderHousingsTable(this.state.rooms);

    return (
      <div>
        <h1 id="tabelLabel" >Rooms</h1>
        <p>This component demonstrates rooms.</p>
        {contents}
      </div>
    );
  }

  async getRoomsData() {
    //const token = await authService.getAccessToken();
      const response = await fetch('/api/rooms/list');//, {
      //headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    //});
      const data = await response.json();
    this.setState({ rooms: data, loading: false });
  }
}
