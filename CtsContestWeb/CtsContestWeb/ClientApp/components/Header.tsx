import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { Icon, Menu, Responsive } from 'semantic-ui-react';
import { Login } from './Login';


// Pictures

const LogoPic: string = require('../assets/CognizantLogo.svg');

//

const links = [
	{
        routeTo: '/',
        name: 'About'
    },
    {
        routeTo: '/tasks',
        name: 'Tasks'
    },
    {
        routeTo: "/puzzles",
        name: "Puzzles"
    },
    // {
    //     routeTo: '/duel',
    //     name: 'duel'
    // },
    // {
    //     routeTo: '/leaderboard',
    //     name: 'Leaderboard'
    // },
    {
        routeTo: '/shop',
        name: 'Shopping booth'
    },
    {
        routeTo: '/prizes',
        name: 'Day prize'
    },
    //{
    //     routeTo: '/quiz',
    //     name: 'Quiz'
    // }
];

export type HeaderState = {
    activeItem: string;
    collapsed: boolean;
    userInfo: boolean;
}

export class Header extends React.Component<any, HeaderState> {

    handleItemClick = (e: any, { name }: any) => this.setState({ activeItem: name })
    constructor(props: any) {
        super(props);

        let collapsed = false;
        if (window.innerWidth <= 1200)
            collapsed = true;

        this.state = {
            activeItem: "tasks",
            collapsed: collapsed,
            userInfo: false
        }
    }

    handleResize = () => {
        if (window.innerWidth >= 1200)
            this.setState({ collapsed: false });
        if (window.innerWidth < 1200)
            this.setState({ collapsed: true });
    }

    handleCollapseMenuButton = () => {
        this.setState({ collapsed: !this.state.collapsed, userInfo: false });
    }
    toggleUserInfo = () => {
        this.setState({ userInfo: !this.state.userInfo, collapsed: true });
    }
    public render() {
        const activeItem = this.state.activeItem;
        return (
            <Menu className="cg-nav" size='large' color='blue' inverted>
                <Responsive maxWidth={1200} onUpdate={this.handleResize} />
                <Menu.Item className='cg-nav-header' header>
                    <div style={{ width: '100%' }}>
                        <div style={{ float: 'left' }}><NavLink to='/' ><img className='cg-nav-logo' src={LogoPic} alt="Cognizant logo" /></NavLink></div>
                        <div className='cg-nav-right'>
                            {this.props.userInfo.isLoggedIn
                             ? <div className='cg-mobile-item'>
                                <Icon link name='user circle' onClick={this.toggleUserInfo} size='big' />
                                <div className={'cg-user-info-menu ' + (this.state.userInfo ? '' : 'hidden')}>
                                    <div>
                                            <div className='cg-user-menu-item cg-bold'>User</div>
                                            <div className='cg-user-menu-item'>{this.props.userInfo.name}</div>
                                            <div className='cg-user-menu-item cg-bold'>My wallet</div>
                                            <div className='cg-user-menu-item'>Total balance: {this.props.userInfo.totalBalance} coins</div>
                                            <a className='cg-user-menu-item cg-bold' href={"https://cts-contest.azurewebsites.net/.auth/logout?post_logout_redirect_uri=" + window.location.pathname}>Logout</a>
                                    </div>
                                </div>
                            </div>
                             : ''
                             }
                            <div className='cg-mobile-item'>
                                <Icon link name='content' onClick={this.handleCollapseMenuButton} size='big' />
                            </div>
                        </div>
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
                        <Login userInfo={this.props.userInfo} />
                    </Menu.Menu>
                    : ""
                }


            </Menu>
        )
    }
}