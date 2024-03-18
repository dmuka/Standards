import React from 'react';
import { Form } from 'react-bootstrap';

const FormGroupComponent = ({ controlId, label, type, name, value, onChange, placeholder, validationError }) => {
    return (
        <Form.Group controlId={controlId}>
            <Form.Label>{label}</Form.Label>
            <Form.Control type={type} name={name} value={value} onChange={onChange} placeholder={placeholder} isInvalid={validationError} />
            <Form.Control.Feedback type="invalid">
                {validationError}
            </Form.Control.Feedback>
        </Form.Group>
    );
};

export default FormGroupComponent;