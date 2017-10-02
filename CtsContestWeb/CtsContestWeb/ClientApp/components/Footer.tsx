import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link, NavLink } from 'react-router-dom';

import { Menu, Sidebar, Container, Grid, List, Segment, Header } from 'semantic-ui-react';
import { Button, Icon } from 'semantic-ui-react';
import { Responsive } from 'semantic-ui-react'



export class Footer extends React.Component<any, {}> {
    public render() {
        return (
            <Segment
                inverted
                vertical
                style={{
                    height: this.props.height,
                    padding: 'auto',
                    marginTop: -this.props.height
                }}
                textAlign='center'
                color='blue'>
                <Container>
                    &copy; Copyright 2017
                </Container>
            </Segment>
        )
    }
}