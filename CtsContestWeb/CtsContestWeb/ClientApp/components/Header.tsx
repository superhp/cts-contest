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
        name: 'Home'
    },
    {
        routeTo: '/about',
        name: 'About'
    },
    {
        routeTo: '/tasks',
        name: 'Tasks'
    }
];

export interface HeaderProps {
    userInfo: UserInfo;
}

export type HeaderState = {
    activeItem: string;
    collapsed: boolean;
}

export class Header extends React.Component<HeaderProps, HeaderState> {

    handleItemClick = (e: any, { name }: any) => this.setState({ activeItem: name })
    constructor(props: HeaderProps) {
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
            <Menu size='large' stackable className="header-nav" color='blue' inverted>
                <Menu.Item header>
                    <div style={{ width: '100%' }}>
                        <div style={{ float: 'left' }}>CtsContestWeb</div>
                        <div style={{ float: 'right' }}>
                            <Responsive maxWidth={768} onUpdate={this.handleResize}>
                                <Icon link name='content' onClick={this.handleCollapseMenuButton} />
                            </Responsive>
                        </div>
                    </div>
                </Menu.Item>

                {!this.state.collapsed ? links.map((value, index) =>
                    <NavLink key={index} className='item' to={value.routeTo} exact activeClassName='active' onClick={this.handleResize}>
                        {value.name}
                    </NavLink>) : ''
                }
                {!this.state.collapsed
                    ?
                    <Menu.Menu position="right">
                        <Login userInfo={this.props.userInfo} />
                    </Menu.Menu>
                    : ""
                }


            </Menu>
        )
    }
}