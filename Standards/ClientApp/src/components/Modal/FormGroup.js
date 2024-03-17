import React from 'react';
import { Form } from 'react-bootstrap';

const FormGroupComponent = ({ controlId, label, type, name, value, onChange }) => {
    return (
        <Form.Group controlId={controlId}>
            <Form.Label>{label}</Form.Label>
            <Form.Control type={type} name={name} value={value} onChange={onChange} />
        </Form.Group>
    );
};

export default FormGroupComponent;