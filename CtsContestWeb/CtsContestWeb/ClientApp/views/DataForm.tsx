import * as React from 'react';
import { Button, Checkbox, Form } from 'semantic-ui-react'

export default class Questions extends React.Component<any, any> {
    render() {
        return (
            <Form className="dataForm">
                <label className='formInfo'>If you would like to get more information about positions in Cognizant, please leave contact information below and we will reach you.</label>
                <label className='star'>Required fields are marked *.</label>
                <Form.Field>
                    <label>First Name *</label>
                    <input placeholder='First Name' />
                </Form.Field>
                <Form.Field>
                    <label>Last Name *</label>
                    <input placeholder='Last Name' />
                </Form.Field>
                <Form.Field>
                    <label>Email address *</label>
                    <input placeholder='Email' />
                </Form.Field>
                <Form.Field>
                    <label>Phone number</label>
                    <input placeholder='Phone' />
                </Form.Field>
                <Form.Field>
                    <label>Study field</label>
                    <input placeholder='e.g. Mathematics' />
                </Form.Field>
                <Form.Field>
                    <label>Degree</label>
                    <input placeholder="e.g. Bachelor's" />
                </Form.Field>
                <Form.Field>
                    <label>Course</label>
                    <input placeholder="e.g. 3" />
                </Form.Field>
                <Form.Field>
                    <Checkbox className='checkBox' label='I agree that Cognizant could use data, provided above, to contact me for career purposes only.' />
                </Form.Field>
                <button className='cg-card-button cyan'>Submit</button>
            </Form>
        );
    }
}
