import React, { useState, useEffect } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import FormGroup from './FormGroup';

export default function HousingModal({ showModal, editedHousing, action, handleClose, handleSaveChanges }) {
    const [housing, setEditedHousing] = useState(editedHousing);
    const [validationError, setValidationError] = useState(false);

    //useEffect(() => {
    //    if (housing !== editedHousing) {
    //        setEditedHousing(editedHousing);
    //    }
    //}, [housing, editedHousing])

    function handleFieldChange(e){
        const { name, value } = e.target;

        if (name === 'floorsCount') {
            let number = Number(value);

            if (!Number.isInteger(number) || number < 1 || number > 1000) {
                setValidationError('Floors count must be a valid.');
                return;
            }
            else {
                setValidationError(false);
            }
        }

        setEditedHousing(prevState => ({
            ...prevState,
            [name]: value
        }));
    }

    return (
        <Modal show={showModal} onHide={handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>{action} housing</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form noValidate>
                    <FormGroup
                        controlId="formName"
                        label="Name"
                        type="text"
                        name="name"
                        value={(housing && housing.name) || ''}
                        onChange={handleFieldChange}
                    />
                    <FormGroup
                        controlId="formShortName"
                        label="Short Name"
                        type="text"
                        name="shortName"
                        value={(housing && housing.shortName) || ''}
                        onChange={handleFieldChange}
                    />
                    <FormGroup
                        controlId="formAddress"
                        label="Address"
                        type="text"
                        name="address"
                        value={(housing && housing.address) || ''}
                        onChange={handleFieldChange}
                    />
                    <FormGroup
                        controlId="formFloorsCount"
                        label="Floors Count"
                        type="number"
                        name="floorsCount"
                        value={(housing && housing.floorsCount) || ''}
                        onChange={handleFieldChange}
                        placeholder="Enter number of floors"
                        validationError={validationError}
                    />
                    <FormGroup
                        controlId="formComments"
                        label="Comments"
                        type="text"
                        name="comments"
                        value={(housing && housing.comments) || ''}
                        onChange={handleFieldChange}
                    />
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>Close</Button>
                <Button variant="primary" onClick={() => handleSaveChanges(housing)}>Save Changes</Button>
            </Modal.Footer>
        </Modal>
    );
}