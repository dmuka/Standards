import React from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import FormGroup from './FormGroup';

class ModalWindow extends React.Component {
    constructor(props) {
        super(props);
        this.state = {};
    }

    componentDidUpdate(prevProps) {
        if (prevProps.editedHousing !== this.props.editedHousing) {
            this.setState({ editedHousing: { ...this.props.editedHousing } });
        }
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

    render() {
        const { show, handleClose, handleSaveChanges } = this.props;
        const { editedHousing } = this.state;

        return (
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Edit Housing</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <FormGroup
                            controlId="formName"
                            label="Name"
                            type="text"
                            name="name"
                            value={(editedHousing && editedHousing.name) || ''}
                            onChange={this.handleFieldChange}
                        />
                        <FormGroup
                            controlId="formShortName"
                            label="Short Name"
                            type="text"
                            name="shortName"
                            value={(editedHousing && editedHousing.shortName) || ''}
                            onChange={this.handleFieldChange}
                        />
                        <FormGroup
                            controlId="formAddress"
                            label="Address"
                            type="text"
                            name="address"
                            value={(editedHousing && editedHousing.address) || ''}
                            onChange={this.handleFieldChange}
                        />
                        <FormGroup
                            controlId="formFloorsCount"
                            label="Floors Count"
                            type="number"
                            name="floorsCount"
                            value={(editedHousing && editedHousing.floorsCount) || ''}
                            onChange={this.handleFieldChange}
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
}

export default ModalWindow;