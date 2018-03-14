import * as React from 'react';
import { Button, Form, Select, Modal } from 'semantic-ui-react'

export default class Questions extends React.Component<any, any> {
    degreeOptions = [
        { key: "b", value: "bachelor", text: "Bachelor's" },
        { key: "m", value: "master", text: "Master's" },
        { key: "d", value: "doctor", text: "Doctor's"}
    ];     
    courseNumberOptions = [
        { key: "1", value: 1, text: "1" }, { key: "2", value: 2, text: "2" },
        { key: "3", value: 3, text: "3" }, { key: "4", value: 4, text: "4"  },
        { key: "5", value: 5, text: "5" }, { key: "6", value: 6, text: "6"  }
    ];

    initialState = {
        name: '', email: '', surname: '', phone: '', courseName: '', degree: '', courseNumber: 0, answer: '', agree: true, openModal: false
    }
    state = this.initialState;

    handleChange = (e: React.FormEvent<HTMLInputElement>) => this.setState({ [e.currentTarget.name]: e.currentTarget.value });

    handleDropdownChange = (e: React.FormEvent<HTMLInputElement>, data: any) => this.setState({ [data.name]: data.value });

    handleSubmit = () => {
        const { name, surname, email, phone, courseName, degree, courseNumber, answer } = this.state;

        const request = {
            Name: name,
            Surname: surname,
            Email: email,
            Phone: phone,
            CourseName: courseName,
            Degree: degree,
            CourseNumber: courseNumber,
            Answer: this.props.answer
        };
        this.setState(this.initialState);
        console.log(request);
        fetch('api/quiz/AddContact', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(
               request
            ),
            credentials: 'include'
        })
            .then(data => {
                this.openModal();
            })
            .catch(error => {
                console.log(error);
                this.setState({ purchaseId: "", purchaseModalState: 'error' });
            });

    }

    handleAgreementChange = (target: any) => {
        var value = this.state.agree;

        this.setState({
            agree: !value
        })
    }

    openModal = () => {
        this.setState({
            openModal: true
        })
    }

    resetQuiz = (event:any, data: any) => {
        this.setState({ 
            openModal: false
        });

        this.props.onSubmit();
    }

    render() {
        const { name, surname, email, phone, courseName, degree, courseNumber, answer } = this.state;

        return (
            <Form className="dataForm" onSubmit={this.handleSubmit}>
                <label className='formInfo'>If you would like to get more information about positions in Cognizant, please leave contact information below and we will reach you.</label>
                <label className='star'>Required fields are marked *.</label>
                <Form.Field>
                    <label>First Name *</label>
                    <input placeholder='First Name' name='name' value={name} onChange={this.handleChange} required />
                </Form.Field>
                <Form.Field>
                    <label>Last Name *</label>
                    <input placeholder='Last Name' name='surname' value={surname} onChange={this.handleChange} required/>
                </Form.Field>
                <Form.Field>
                    <label>Email address *</label>
                    <input type="email" placeholder='Email' name='email' value={email} onChange={this.handleChange} required/>
                </Form.Field>
                <Form.Field>
                    <label>Phone number</label>
                    <input placeholder='Phone' name='phone' value={phone} onChange={this.handleChange}/>
                </Form.Field>
                <Form.Field>
                    <label>Study field</label>
                    <input placeholder='e.g. Mathematics' name='courseName' value={courseName} onChange={this.handleChange}/>
                </Form.Field>
                <Form.Field>
                    <label>Current studies degree</label>
                    <Select placeholder='Select degree' fluid selection options={this.degreeOptions} name='degree' value={degree} onChange={this.handleDropdownChange} />
                </Form.Field>
                <Form.Field>
                    <label>Course</label>
                    <Select placeholder='Select course' fluid selection options={this.courseNumberOptions} name='courseNumber' value={courseNumber} onChange={this.handleDropdownChange} />
                </Form.Field>
                <Form.Field>
                    <Form.Checkbox className='checkBox' checked={this.state.agree} onChange={this.handleAgreementChange} label='I agree that Cognizant could use data, provided above, to contact me for career purposes only.' required />
                </Form.Field>
                <button className='cg-card-button cyan' disabled={!this.state.agree}>Submit</button>

                <Modal className='cg-modal' style={{ height: '182px' }}
                    size="mini"
                    open={this.state.openModal}
                    onClose={this.resetQuiz}
                    header='Thank you!'
                    content='We will contact you!'
                    actions={[
                        { key: 'done', content: 'OK', positive: true },
                    ]}
                />
            </Form>
        );
    }
}

interface ContactInfo {
    name: string,
    surname: string,
    email: string,
    phone: string,
    courseName: string,
    degree: string,
    courseNumber: number,
    answer: string
}
