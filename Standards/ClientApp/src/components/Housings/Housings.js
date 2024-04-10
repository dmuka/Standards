import React, { useState, useEffect } from 'react'
import ApiRoutes from '../../ApiRoutes'
import authService from '../api-authorization/AuthorizeService'
import { Button } from 'react-bootstrap'
import HousingModal from '../Modal/HousingModal'
import { useTokens } from '../api-authorization/TokensService'

export default function Housings () {
  const [housings, setHousings] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [modalData, setModalData] = useState({})
  const { checkTokenExpiration, getAccessToken } = useTokens();

  useEffect(() => {
    checkTokenExpiration();
  }, []);
  
  useEffect(() => {
    getAccessToken();
  }, []);

  useEffect(() => {
    async function fetchData () {
      setHousings(await getHousingsData())
    }

    fetchData()
  }, [])

  function handleAddHousing () {
    setShowModal(true)
    setModalData({
      editedHousing: {},
      action: 'Add',
      handleSaveChanges: handleAdd
    })
  }

  function handleEditHousing (housing) {
    setShowModal(true)
    setModalData({
      editedHousing: housing,
      action: 'Edit',
      handleSaveChanges: handleEdit
    })
  }

  async function handleDeleteHousing (id) {
    try {
      const response = await fetch(ApiRoutes.HOUSINGS_DELETE + `${id}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getAccessToken()}`
        }
      })

      if (response.ok) {
        setShowModal(false)
        setHousings(housings.filter(h => h.id !== id))
      } else {
        console.error('Failed to save data:', response.statusText)
      }
    } catch (error) {
      console.error('Error saving data:', error)
    }
  }

  function handleCloseModal () {
    setShowModal(false)
  }

  async function handleEdit (entity) {
    try {
      const response = await fetch(ApiRoutes.HOUSINGS_PUT + `${entity.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getAccessToken()}`
        },
        body: JSON.stringify(entity)
      })

      if (response.ok) {
        const editedEntityIndex = housings.findIndex(h => h.id === entity.id)
        const updatedEntities = [...housings]
        updatedEntities[editedEntityIndex] = entity
        setShowModal(false)
        setHousings(updatedEntities)
      } else {
        console.error('Failed to save data:', response.statusText)
      }
    } catch (error) {
      console.error('Error saving data:', error)
    }
  }

  async function handleAdd (addedEntity) {
    try {
      const response = await fetch(ApiRoutes.HOUSINGS_ADD, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getAccessToken()}`
        },
        body: JSON.stringify(addedEntity)
      })

      if (response.ok) {
        setShowModal(false)
        setHousings(await getHousingsData())
      } else {
        console.error('Failed to save data:', response.statusText)
      }
    } catch (error) {
      console.error('Error saving data:', error)
    }
  }

  async function getHousingsData () {
    try {
      const response = await fetch(ApiRoutes.HOUSINGS_LIST, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getAccessToken()}`
        }        
      })

      if (!response.ok) {
        throw new Error('Failed to fetch housings data')
      }

      return await response.json()
    } catch (error) {
      console.error('Error fetching housings data:', error)
    }
  }

  function renderHousingsTable () {
    return (
            <div>
                <div><Button onClick={handleAddHousing}>Add</Button></div>
                <table className='table table-striped' aria-labelledby="tableLabel">
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
                            </tr>
                        }
                        )}
                    </tbody>
                </table>
            </div>
    )
  }

  return (
        <div>
            <h1 id="tableLabel">Housings</h1>
            {renderHousingsTable()}
            {showModal &&
                <HousingModal
                    showModal={true}
                    handleClose={handleCloseModal}
                    editedHousing={modalData.editedHousing}
                    action={modalData.action}
                    handleSaveChanges={modalData.handleSaveChanges}
                />
            }
        </div>
  )
}
