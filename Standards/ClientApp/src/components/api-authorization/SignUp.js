import React, { useState } from "react";
import { Form, Button } from "react-bootstrap";
import Login from "./Login";
import FormGroup from "../Modal/FormGroup";

export default function SignUp({ retUrl }) {
  const returnUrl = retUrl;

  const [formData, setFormData] = useState({
    userName: "",
    email: "",
    password: "",
    reEnterPassword: "",
  });

  const [errors, setErrors] = useState({});

  function validateForm() {
    let isValid = true;
    const currentErrors = {};

    if (formData.email) {
      const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

      if (!emailPattern.test(val)) {
        currentErrors.email = "You must enter a valid email address.";
        isValid = false;
      }
    }

    if (
      formData.userName &&
      (formData.userName.length < 3 || formData.userName.length > 50)
    ) {
      currentErrors.userName = "You must enter valid user name.";
      isValid = false;
    }

    if (
      formData.password &&
      (formData.password < 2 || formData.password > 10)
    ) {
      currentErrors.userName = "You must enter valid password.";
      isValid = false;
    }

    if (
      formData.reEnterPassword &&
      formData.reEnterPassword != formData.password
    ) {
      currentErrors.reEnterPassword =
        "You must re-enter correct value of password.";
      isValid = false;
    }

    setErrors(currentErrors);

    return isValid;
  }

  function handleFieldChange(event) {
    const { name, value } = event.target;

    setFormData({
      ...formData,
      [name]: value,
    });
  }

  function handleSubmit(event) {
    const form = event.currentTarget;

    if (validateForm()) {
      event.preventDefault();
      event.stopPropagation();
    }

  }

  function handleClose() {
    window.location.replace(returnUrl);
  }

  return (
    <div>
      <Form noValidate>
        <h3 className="Auth-form-title">Sign up</h3>
        <FormGroup
          controlId="formUserName"
          label="User name"
          required
          placeholder="Enter user name"
          type="text"
          name="userName"
          onChange={handleFieldChange}
          validationError={errors.userName}
        />
        <FormGroup
          controlId="formEmail"
          label="E-mail address"
          required
          placeholder="E-mail address"
          type="text"
          name="email"
          onChange={handleFieldChange}
          validationError={errors.email}
        />
        <FormGroup
          controlId="password"
          label="Password"
          required
          placeholder="Enter password"
          type="password"
          name="password"
          onChange={handleFieldChange}
          validationError={errors.password}
        />
        <FormGroup
          controlId="reEnterPassword"
          label="Re-enter password"
          required
          placeholder="Re-enter password"
          type="password"
          name="reEnterPassword"
          onChange={handleFieldChange}
          validationError={errors.reEnterPassword}
        />
        <Button variant="primary" onClick={() => handleSaveChanges(housing)}>
          Sign up
        </Button>
        <Button variant="secondary" onClick={handleClose}>
          Close
        </Button>
      </Form>
    </div>
  );
}
