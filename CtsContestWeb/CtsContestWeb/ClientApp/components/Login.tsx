import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Label, Button, Header as Header, Image, Modal, Icon } from 'semantic-ui-react'
import { Link, NavLink } from 'react-router-dom';

export interface LoginProps {
    userInfo: UserInfo;
}

export class Login extends React.Component<LoginProps, {}> {
    constructor(props: LoginProps) {
        super(props);
    }

    public render() {
        let contents = this.props.userInfo.isLoggedIn
            ? Login.renderLoggedInView(this.props.userInfo)
            : Login.renderLoginModal()

        return contents;
    }

    private static renderLoggedInView(userInfo: UserInfo) {
        return <div className="right-menu"><a className='item'>Hello, {userInfo.name}!</a> <a className='item' href="https://cts-contest.azurewebsites.net/.auth/logout?post_logout_redirect_uri=/">Logout</a></div>;
    }

    private static renderLoginModal() {
        return <Modal size="tiny" className="login-modal" trigger={<NavLink className='item' to="#" exact> Login </NavLink>} closeIcon>
            <Modal.Header>Choose login method</Modal.Header>
            <Modal.Content>
            <Modal.Description>
                <a href="https://cts-contest.azurewebsites.net/.auth/login/facebook?post_login_redirect_url=/"><Button color='facebook'><Icon name='facebook' /> Login with Facebook</Button></a>
                <a href="https://cts-contest.azurewebsites.net/.auth/login/google?post_login_redirect_url=/"><Button color='google plus'><Icon name='google plus' /> Login with Google Plus</Button></a>
            </Modal.Description>
            </Modal.Content>
        </Modal>;
    }
}
