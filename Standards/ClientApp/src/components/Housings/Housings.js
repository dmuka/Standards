import React, { useState, useEffect } from 'react';
import ApiRoutes from '../../ApiRoutes';
import authService from '../api-authorization/AuthorizeService'
import { Button } from 'react-bootstrap';
import ModalWindow from '../Modal/ModalWindow';

export default function Housings() {
    const [housings, setHousings] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const [action, setAction] = useState('');
    const [editedHousing, setEditedHousing] = useState({ });

    useEffect(() => {
        async function fetchData() {
            setHousings(await getHousingsData());
        }

        fetchData();
    }, []);

    function handleAddHousing() {
        setEditedHousing({});
        setAction('Add');
        setShowModal(true);
    }

    function handleEditHousing(housing) {
        setEditedHousing(housing);
        setAction('Edit');
        setShowModal(true);
    }

    async function handleDeleteHousing(id){
        try {
            const response = await fetch(ApiRoutes.HOUSINGS_DELETE + `${id}`, {
                method: 'DELETE',
                headers: {
                    //'Content-Type': 'application/json',
                    // Include authorization token if needed
                    // 'Authorization': `Bearer ${token}`
                }
            });

            if (response.ok) {
                setShowModal(false);
                setHousings(housings.filter(h => h.id !== id));
            } else {
                console.error('Failed to save data:', response.statusText);
            }
        } catch (error) {
            console.error('Error saving data:', error);
        }
    }

    function handleCloseModal(){
        setShowModal(false);
    }

    function handleFieldChange(e){
        const { name, value } = e.target;
        return {
            editedHousing: {
                [name]: value
            }
        }
    }

    async function handleEdit(entity){
        try {
            const response = await fetch(ApiRoutes.HOUSINGS_PUT + `${entity.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    // Include authorization token if needed
                    // 'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(entity)
            });

            if (response.ok) {
                const editedEntityIndex = housings.findIndex(h => h.id === entity.id);
                const updatedEntities = [...housings];
                updatedEntities[editedEntityIndex] = entity;
                setShowModal(false);
                setHousings(updatedEntities);
            } else {
                console.error('Failed to save data:', response.statusText);
            }
        } catch (error) {
            console.error('Error saving data:', error);
        }
    }

    async function handleAdd(addedEntity){
        try {
            const response = await fetch(ApiRoutes.HOUSINGS_ADD, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    // Include authorization token if needed
                    // 'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(addedEntity)
            });

            if (response.ok) {
                setShowModal(false);
                setHousings(await getHousingsData());
            } else {
                console.error('Failed to save data:', response.statusText);
            }
        } catch (error) {
            console.error('Error saving data:', error);
        }
    }

    async function getHousingsData() {
        try {
            const response = await fetch(ApiRoutes.HOUSINGS_LIST);

            if (!response.ok) {
                throw new Error('Failed to fetch housings data');
            }

            return await response.json();
        }
        catch (error) {
            console.error('Error fetching housings data:', error);
        }
    }

    function renderHousingsTable() {
        return (
            <div>
                <div><Button onClick={handleAddHousing}>Add</Button></div>
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
                                <td><Button onClick={() => handleEditHousing(housing)}>Edit</Button></td>
                                <td><Button onClick={() => handleDeleteHousing(housing.id)}>Delete</Button></td>
                            </tr>;
                        }
                        )}
                    </tbody>
                </table>
            </div>
        )
    }

        return (
            <div>
              <h1 id="tabelLabel">Housings</h1>
              <p>This component demonstrates housings.</p>
              {renderHousingsTable()}
              <ModalWindow
                  show={showModal}
                  editedHousing={editedHousing}
                  action={action}
                  handleClose={handleCloseModal}
                  handleFieldChange={handleFieldChange}
                  handleSaveChanges={action === 'Edit' ? handleEdit : handleAdd }
              />
          </div>
      )
}