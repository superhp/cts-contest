import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link, NavLink } from 'react-router-dom';

import { Menu, Sidebar, Container, Segment } from 'semantic-ui-react';
import { Button, Icon } from 'semantic-ui-react';
import { Responsive } from 'semantic-ui-react'
import { Header as ModalHeader, Image, Modal } from 'semantic-ui-react'
import { Login } from './login';

const links = [
    {
        routeTo: '/',
        name: 'Tasks'
    },
    {
        routeTo: '/prizes',
        name: 'Prizes'
    },
    {
        routeTo: '/about',
        name: 'About'
    }
];

export type HeaderState = {
    activeItem: string;
    collapsed: boolean;
}

export class Header extends React.Component<any, HeaderState> {

    handleItemClick = (e: any, { name }: any) => this.setState({ activeItem: name })
    constructor(props:any) {
        super(props);

        let collapsed = false;
        if (window.innerWidth <= 768)
            collapsed = true;

        this.state = {
            activeItem: "tasks",
            collapsed: collapsed
        }
    }
    
    handleResize = () => {
        if (window.innerWidth >= 768)
            this.setState({ collapsed: false });
        if (window.innerWidth < 768)
            this.setState({ collapsed: true });
    }

    handleCollapseMenuButton = (e: any, { name }: any) => {
        this.setState({ collapsed: !this.state.collapsed });
    }

    public render() {
        const activeItem = this.state.activeItem;
        return (
            <Menu className="cg-nav" size='large' stackable color='blue' inverted>
                <Menu.Item className='cg-nav-header' header>
                    <div style={{ width: '100%' }}>
                    <div style={{ display: 'inline' }}><NavLink to='/' ><img className='cg-nav-logo' src="../logo.svg" alt="Cognizant logo"/></NavLink></div>
                    <div style={{ position: 'absolute', right: 10, top: 15, display: 'inline', margin: 'auto' }}>
                         <Responsive maxWidth={768} onUpdate={this.handleResize}>
                                <div>
                                    <Icon link name='content' onClick={this.handleCollapseMenuButton} size='big'/>
                                </div>
                        </Responsive>
                    </div>
                    {/* <div style={{ width: '100%' }}>
                        <div style={{ float: 'left' }}>CtsContestWeb</div>
                        <div style={{ float: 'right' }}>
                            <Responsive maxWidth={768} onUpdate={this.handleResize}>
                                <Icon link name='content' onClick={this.handleCollapseMenuButton} />
                            </Responsive>
                        </div>
                    </div> */}
                    </div>
                </Menu.Item>

                {!this.state.collapsed ? links.map((value, index) =>
                    <NavLink key={index} className='item cg-nav-item' to={value.routeTo} exact activeClassName='active' onClick={this.handleResize}>
                        {value.name}
                    </NavLink>) : ''
                }
                {!this.state.collapsed
                    ?
                    <Menu.Menu position="right">
                        <Login userInfo={this.props.userInfo}/>
                    </Menu.Menu>
                    : ""
                }


            </Menu>
        )
    }
}