import React, { useState } from 'react'
import { Form, FormGroup } from 'react-bootstrap'

export default function Sign (props) {
  const [mode, setMode] = useState(props.mode)
  const [validationError, setValidationError] = useState()

  function changeMode () {
    return setMode(props.mode === 'SignIn' ? 'SignUp' : 'SignIn')
  }

  function handleFieldChange (event) {
    const { name, value } = event.target

    const val = String(value)

    if (name === 'firstName') {
      if (val === 'undefined' || val === 'null' || val.length < 2 || val.length > 50) {
        setValidationError('You must enter valid first name.')
      } else {
        setValidationError(false)
      }
    }
  }

  function handleSubmit (event) {
    const form = event.currentTarget
    if (form.checkValidity() === false) {
      event.preventDefault()
      event.stopPropagation()
    }

    setValidated(true)
  }

  return props.mode === 'SignIn'
    ? (
            <div className="Auth-form-container">
              <form className="Auth-form" noValidate onSubmit={handleSubmit}>
                    <div className="Auth-form-content">
                        <h3 className="Auth-form-title">Sign in</h3>
                        <div className="text-center">
                            Not registered yet?{' '}
                            <span className="link-primary" onClick={changeMode}>
                                Sign up
                            </span>
                        </div>
                        <div className="form-group mt-3">
                            <label>Email address</label>
                          <input
                            required
                                type="email"
                                className="form-control mt-1"
                                placeholder="Enter email"
                            />
                        </div>
                        <div className="form-group mt-3">
                            <label>Password</label>
                          <input
                            required
                                type="password"
                                className="form-control mt-1"
                                placeholder="Enter password"
                            />
                        </div>
                        <div className="d-grid gap-2 mt-3">
                            <button type="submit" className="btn btn-primary">
                                Submit
                            </button>
                        </div>
                        <p className="text-center mt-2">
                            Forgot <a href="#">password?</a>
                        </p>
                    </div>
                </form>
            </div>
      )
    : (
          <>
                <Form noValidate>
                  <FormGroup
                      controlId="formFirstName"
                      label="First name"
                      required
                      placeholder="Enter first name"
                      type="text"
                      name="firstName"
                      validationError={validationError}
                      onChange={handleFieldChange}
                  />
                  <Form.Control.Feedback type="invalid">
                      Please enter first name.
                  </Form.Control.Feedback>
                    <FormGroup
                      controlId="formLastName"
                      label="Last name"
                      required
                      placeholder="Enter last name"
                      type="text"
                      name="lastName"
                      onChange={handleFieldChange}
                    />
                  <FormGroup
                      controlId="password"
                      label="Password"
                      required
                      type="password"
                      name="password"
                      onChange={handleFieldChange}
                    />
                  <FormGroup
                      controlId="formFloorsCount"
                      label="Floors Count"
                      type="number"
                      name="floorsCount"
                      onChange={handleFieldChange}
                      placeholder="Enter number of floors"
                      validationError={validationError}
                    />
              </Form>
          </>
      )
}
