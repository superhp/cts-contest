import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Responsive, Label, Button, Header as Header, Image, Modal, Icon, Menu } from 'semantic-ui-react'
import { Link, NavLink } from 'react-router-dom';

interface LoginModalState {
    modalHeight: number;
    loading: boolean;
    wallet: boolean;
}

export class Login extends React.Component<any, LoginModalState> {
    constructor(props:any) {
        super(props);

        let modalHeight = 200;
        if (window.innerWidth < 960)
            modalHeight = 250;

        this.state = {
            modalHeight: modalHeight,
            loading: true,
            wallet: false
        }

        this.handleResize = this.handleResize.bind(this);
    }

    handleResize = () => {
        if (window.innerWidth < 960) {
            this.setState({ modalHeight: 250 });
        } else
            this.setState({ modalHeight: 200 });
    }
    toggleWallet = () => {
        this.setState({ wallet: !this.state.wallet });
    }
    public render() {
        let contents = false
            ? this.renderLoggedInView(this.props.userInfo)
            : Login.renderLoginModal(this.state.modalHeight, this.handleResize)

        return contents;
    }

    private renderLoggedInView(userInfo: any) {
        return (
            <div className="right-menu">
                <div className='item cg-responsive-hide'>Hello, {userInfo.name}!</div>
                <div style={{position: 'relative'}}>
                    <a className={"item cg-responsive-hide cg-bold " + (this.state.wallet ? 'active' : '')} onClick={this.toggleWallet} style={{height: '100%', width: '100%'}}>My Wallet</a>
                    <div className={'cg-balance ' + (this.state.wallet ? 'cg-show' : 'cg-hidden')}>
                        <table>
                            <tbody>
                                <tr>
                                    <td>Todays balance</td>
                                    <td>{this.props.userInfo.todaysBalance} pts</td>
                                </tr>
                                <tr>
                                    <td>Tatal balance</td>
                                    <td>{this.props.userInfo.totalBalance} pts</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <a className='item cg-responsive-hide cg-bold' href={"https://cts-contest.azurewebsites.net/.auth/logout?post_logout_redirect_uri=" + window.location.pathname}>Logout</a>
            </div>
        );
    }

    private static renderLoginModal(height: number, handleResize: any) {
        return <Responsive className='cg-login-mobile' onUpdate={handleResize}>
            <Modal size="tiny" className="login-modal" trigger={<NavLink style={{height: '100%'}} className='item cg-nav-item' to="#" exact> Login </NavLink>} style={{ minHeight: height }} closeIcon>
                <Modal.Header>Choose login method</Modal.Header>
                <Modal.Content>
                    <Modal.Description>
                        <a href={"https://cts-contest.azurewebsites.net/.auth/login/facebook?post_login_redirect_url=" + window.location.pathname}><Button color='facebook'><Icon name='facebook' /> Login with Facebook</Button></a>
                        <a href={"https://cts-contest.azurewebsites.net/.auth/login/google?post_login_redirect_url=" + window.location.pathname}><Button color='google plus'><Icon name='google' /> Login with Google</Button></a>
                    </Modal.Description>
                </Modal.Content>
            </Modal>
        </Responsive>;
    }
}
