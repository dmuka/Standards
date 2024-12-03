import React, { useState, useContext } from "react";
import { Form, Button } from "react-bootstrap";
import { useAuth } from "./AuthProvider";
import Login from "./Login";
import FormGroup from "../Modal/FormGroup";

export default function SignIn({ returnUrl }) {
  const [formData, setFormData] = useState({
    userName: "",
    password: "",
  });

  const auth = useAuth();

  const [errors, setErrors] = useState({});

  function validateForm() {
    let isValid = true;
    const currentErrors = {};

    if (formData.userName.includes("@")) {
      const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

      if (!emailPattern.test(val)) {
        currentErrors.email = "You must enter a valid email address.";
        isValid = false;
      }
    } else if (
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

  function handleSubmit() {
    if (validateForm()) {
      auth.loginAction(formData);
    }
  }

  function handleClose() {
    window.location.replace(returnUrl);
  }

  return (
    <div className="Auth-form-container">
      <Form noValidate>
        <h3 className="Auth-form-title">Sign in</h3>
        <FormGroup
          controlId="formUserName"
          label="User name or e-mail address"
          required
          placeholder="Enter user name or e-mail address"
          type="text"
          name="userName"
          onChange={handleFieldChange}
          validationError={errors.userName}
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
      </Form>
      <p className="text-center mt-2">
        Forgot <a href="#">password?</a>
      </p>
      <Button variant="primary" onClick={handleSubmit}>
        Sign in
      </Button>
      <Button variant="secondary" onClick={handleClose}>
        Close
      </Button>
    </div>
  );
}
