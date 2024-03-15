import React from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

class ModalWindow extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            editedHousing: { ...props.editedHousing } // Initialize state with props.editedHousing
        };
    }

    // Update state when props change
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
                        <Form.Group controlId="formName">
                            <Form.Label>Name</Form.Label>
                            <Form.Control type="text" name="name" value={(editedHousing && editedHousing.name) || ''} onChange={this.handleFieldChange} />
                        </Form.Group>
                        <Form.Group controlId="formShortName">
                            <Form.Label>Short Name</Form.Label>
                            <Form.Control type="text" name="shortName" value={(editedHousing && editedHousing.shortName) || ''} onChange={this.handleFieldChange} />
                        </Form.Group>
                        <Form.Group controlId="formAddress">
                            <Form.Label>Address</Form.Label>
                            <Form.Control type="text" name="address" value={(editedHousing && editedHousing.address) || ''} onChange={this.handleFieldChange} />
                        </Form.Group>
                        <Form.Group controlId="formFloorsCount">
                            <Form.Label>Floors Count</Form.Label>
                            <Form.Control type="number" name="floorsCount" value={(editedHousing && editedHousing.floorsCount) || ''} onChange={this.handleFieldChange} />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Close</Button>
                    <Button variant="primary" onClick={handleSaveChanges}>Save Changes</Button>
                </Modal.Footer>
            </Modal>
        );
    }
}

export default ModalWindow;