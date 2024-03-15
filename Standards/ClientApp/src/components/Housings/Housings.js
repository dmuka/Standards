import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService'
import { Button } from 'react-bootstrap';
import ModalWindow from '../Modal/ModalWindow';

export class Housings extends Component {
  static displayName = Housings.name;

  constructor(props) {
      super(props);
      this.state = { housings: [], showModal: false, editedHousing: {} };
  }

    async componentDidMount() {
      try {
          const response = await fetch('/api/housings/list');

          if (!response.ok) {
              throw new Error('Failed to fetch housings data');
          }

          const data = await response.json();

          this.setState({ housings: data });
      }
      catch (error) {
          console.error('Error fetching housings data:', error);
          // Handle error state, display error message, etc.
      }
    }

    handleAddHousing(){
        this.setState({ showModal: true });
    }

    handleEditHousing(housing){
        this.setState({ editedHousing: { ...housing }, showModal: true });
    }

    handleDeleteHousing = (id) => {
        // Implement the logic to handle deleting the housing with the given id
        console.log("Delete housing with ID:", id);
    }

    handleCloseModal = () => {
        this.setState({ showModal: false });
    }

    handleFieldChange = (e) => {
        const { name, value } = e.target;
        this.setState(prevState => ({
            editedHousing: {
                ...prevState.editedHousing,
                [name]: value
            }
        }));
    }

    handleSaveChanges = () => {
        // Send editedHousing to backend for saving
        console.log("Saving changes:", this.state.editedHousing);
        // Close modal after saving
        this.setState({ showModal: false });
    }

  renderHousingsTable(housings) {
      return (
          <div>
              <div><Button onClick={() => this.handleAddHousing()}>Add</Button></div>
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
                          <td><Button onClick={() => this.handleEditHousing(housing)}>Edit</Button></td>
                          <td><Button onClick={() => this.handleDeleteHousing(housing.id)}>Delete</Button></td>
                      </tr>;
                  }
                  )}
              </tbody>
              </table>
          </div>
    );
    }

  render() {
      const { housings, showModal, editedHousing } = this.state;

      let contents = this.renderHousingsTable(housings);

      return (
          <div>
              <h1 id="tabelLabel">Housings</h1>
              <p>This component demonstrates housings.</p>
              {contents}
              <ModalWindow
                  show={showModal}
                  editedHousing={editedHousing}
                  handleClose={this.handleCloseModal}
                  handleFieldChange={this.handleFieldChange}
                  handleSaveChanges={this.handleSaveChanges}
              />
          </div>
          );
      }
}