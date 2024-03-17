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

    componentDidMount() {
        this.getHousingsData();
    }

    //async componentDidMount() {
    //    getHousingsData();
    //}

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

    handleSaveChanges = async (editedEntity, entitiesCollectionName, entitiesCollection) => {
        try {
            const response = await fetch(`/api/housings/edit/${editedEntity.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    // Include authorization token if needed
                    // 'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(editedEntity)
            });

            if (response.ok) {
                const editedEntityIndex = this.state.housings.findIndex(h => h.id === editedEntity.id);
                const updatedEntities = [...this.state.housings];
                updatedEntities[editedEntityIndex] = editedEntity;
                this.setState({ showModal: false, "{ entitiesCollectionName }": updatedEntities });
            } else {
                console.error('Failed to save data:', response.statusText);
            }
        } catch (error) {
            console.error('Error saving data:', error);
        }
    }

    async getHousingsData() {
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
        }
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