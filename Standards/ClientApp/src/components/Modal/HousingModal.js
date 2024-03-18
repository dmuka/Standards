import React, { useState, useEffect } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import FormGroup from './FormGroup';

export default function HousingModal({ showModal, editedHousing, action, handleClose, handleSaveChanges }) {
    const [housing, setEditedHousing] = useState(editedHousing);
    const [validationError, setValidationError] = useState(false);

    useEffect(() => {
        if (housing !== editedHousing) {
            setEditedHousing(editedHousing);
        }
    }, [housing, editedHousing])

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

        return {
            editedHousing: {
                [name]: value
            }
        };
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
                        value={(editedHousing && editedHousing.name) || ''}
                        onChange={handleFieldChange}
                    />
                    <FormGroup
                        controlId="formShortName"
                        label="Short Name"
                        type="text"
                        name="shortName"
                        value={(editedHousing && editedHousing.shortName) || ''}
                        onChange={handleFieldChange}
                    />
                    <FormGroup
                        controlId="formAddress"
                        label="Address"
                        type="text"
                        name="address"
                        value={(editedHousing && editedHousing.address) || ''}
                        onChange={handleFieldChange}
                    />
                    <FormGroup
                        controlId="formFloorsCount"
                        label="Floors Count"
                        type="number"
                        name="floorsCount"
                        value={(editedHousing && editedHousing.floorsCount) || ''}
                        onChange={handleFieldChange}
                        placeholder="Enter number of floors"
                        validationError={validationError}
                    />
                    <FormGroup
                        controlId="formComments"
                        label="Comments"
                        type="text"
                        name="comments"
                        value={(editedHousing && editedHousing.comments) || ''}
                        onChange={handleFieldChange}
                    />
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>Close</Button>
                <Button variant="primary" onClick={() => handleSaveChanges(editedHousing)}>Save Changes</Button>
            </Modal.Footer>
        </Modal>
    );
}