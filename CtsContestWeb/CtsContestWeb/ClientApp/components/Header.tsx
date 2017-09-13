import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link, NavLink } from 'react-router-dom';

import { Menu, Sidebar, Container, Segment } from 'semantic-ui-react';
import { Button, Icon } from 'semantic-ui-react';
import { Responsive } from 'semantic-ui-react'

const links = [{
    routeTo: '/',
    name: 'Tasks'
    },
    {
    routeTo: '/about',
    name: 'About'
    }
]

export type HeaderState = {
    activeItem: string;
    collapsed: boolean;
}

export class Header extends React.Component<{}, HeaderState> {

    handleItemClick = (e: any, { name }: any) => this.setState({ activeItem: name })
    constructor() {
        super();
        this.state = {
            activeItem: "tasks",
            collapsed: false
        }
        if (window.innerWidth <= 768) {
            this.setState({ collapsed: true });
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
            <Menu size='large' stackable inverted className="header-nav" color='blue'>
               
                    <Menu.Item header>
                        CtsContestWeb
                         <Responsive maxWidth={768} onUpdate={this.handleResize}>
                            <Icon link name='content' onClick={this.handleCollapseMenuButton} />
                        </Responsive>
                    </Menu.Item>
                    {!this.state.collapsed?links.map(value => 
                        <NavLink className='item' to={value.routeTo} exact activeClassName='active' onClick={this.handleResize}>
                            {value.name}
                        </NavLink>):''
                    }
                    {!this.state.collapsed
                        ?
                        <Menu.Menu position="right">
                            <Menu.Item>
                                <Button circular color='facebook' icon='facebook' />

                                <Button circular color='google plus' icon='google plus' />
                            </Menu.Item>
                        </Menu.Menu>
                        : ""
                    }

                
            </Menu>
        )
    }
}