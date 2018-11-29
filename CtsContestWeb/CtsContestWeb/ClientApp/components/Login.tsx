
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Responsive, Label, Button, Header as Header, Image, Modal, Icon, Menu, Checkbox } from 'semantic-ui-react'
import { Link, NavLink } from 'react-router-dom';

interface LoginModalState {
    modalHeight: number;
    loading: boolean;
    wallet: boolean;
    ConsentGiven: boolean;
    totalWins: number;
    totalLoses: number;
}

export class Login extends React.Component<any, LoginModalState> {
    constructor(props: any) {
        super(props);

        let modalHeight = 200;
        if (window.innerWidth < 960)
            modalHeight = 250;

        this.state = {
            modalHeight: modalHeight,
            loading: true,
            wallet: false,
            ConsentGiven: false,
            totalWins: 0,
            totalLoses: 0
        }

        this.handleResize = this.handleResize.bind(this);

        if (window.location.search.indexOf("refresh=true") !== -1) {
            window.location.replace(window.location.origin + window.location.pathname);
        }       
        
        //setInterval(this.updateDuelStatistics, 10 * 1000);
    }

    handleResize = () => {
        if (window.innerWidth < 960) {
            this.setState({ modalHeight: 200 });
        } else
            this.setState({ modalHeight: 200 });
    }
    toggleWallet = () => {
        this.setState({ wallet: !this.state.wallet });
    }

    updateDuelStatistics = () => {
        fetch('api/user/duel-statistics', {
            credentials: 'include'
        }).then(response => response.json() as Promise<any>)
            .then(data => {
                this.setState({
                    totalWins: data.totalWins,
                    totalLoses: data.totalLooses
                });
            });
    }

    public render() {
        let contents = this.props.userInfo.isLoggedIn
            ? this.renderLoggedInView(this.props.userInfo, this.detectIE())
            : Login.renderLoginModal(this.state.modalHeight, this.detectIE(), this.handleResize, this.state.ConsentGiven, this.ToggleConsentCheckbox)

        return contents;
    }

    private detectIE() {
        var ua = window.navigator.userAgent;

        var msie = ua.indexOf('MSIE ');
        if (msie > 0) {
            // IE 10 or older => return version number
            return true;
        }

        var trident = ua.indexOf('Trident/');
        if (trident > 0) {
            // IE 11 => return version number
            var rv = ua.indexOf('rv:');
            return true;
        }

        var edge = ua.indexOf('Edge/');
        if (edge > 0) {
            // Edge (IE 12+) => return version number
            return true;
        }

        // other browser
        return false;
    }

    private renderLoggedInView(userInfo: any, isIE: boolean) {
        var addon = "";
        if (isIE) {
            addon = "?refresh=true";
        }

        return (
            <div className="right-menu">
                <div className='item cg-responsive-hide'>
                    <div className='cg-username'>{userInfo.name}</div>
                    {/* <div className='cg-label-container'>
                        <div className='ui blue label cg-label'>
                            Duels won <span className='detail cg-label-detail'>{this.state.totalWins}</span>
                        </div>
                        <div className='ui blue label cg-label'>
                            Duels lost <span className='detail cg-label-detail'>{this.state.totalLoses}</span>
                        </div>
                    </div> */}
                </div>
                <div style={{ position: 'relative' }}>
                    <a className={"item cg-responsive-hide cg-bold " + (this.state.wallet ? 'active' : '')} onClick={this.toggleWallet} style={{ height: '100%', width: '100%' }}>My Wallet</a>
                    <div className={'cg-balance ' + (this.state.wallet ? 'cg-show' : 'cg-hidden')}>
                        <table>
                            <tbody>
                                {/* <tr>
                                    <td>Days balance</td>
                                    <td>{this.props.userInfo.todaysBalance} coins</td>
                                </tr> */}
                                <tr>
                                    <td>Total balance</td>
                                    <td>{this.props.userInfo.totalBalance} coins</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <a className='item cg-responsive-hide cg-bold' href={"https://contest-cts.azurewebsites.net/.auth/logout?post_logout_redirect_uri=" + window.location.pathname  + addon}>Logout</a>
            </div>
        );
    }

    private static renderLoginModal(height: number, isIE: boolean, handleResize: any, ConsentGiven: boolean, ToggleConsentCheckbox: () => void) {

        var addon = "";
        if (isIE) {
            addon = "?refresh=true";
        }
    
        return <Responsive className='cg-login-mobile' onUpdate={handleResize}>
            <Modal size="tiny" className="login-modal" trigger={<NavLink style={{ height: '100%' }} className='item cg-nav-item' to="#" exact> Login </NavLink>} closeIcon>
                <Modal.Header>Choose login method</Modal.Header>

                
                <div style={{ padding: "15px", textAlign: "justify" }}>
                    <Checkbox
                        checked={ConsentGiven}
                        onChange={() => { ToggleConsentCheckbox(); }}
                        label={{children:<p>By clicking this I agree to the <NavLink to={'/privacyPolicy'} target='_blank'>terms of service</NavLink></p>}}
                    />
                </div>
            
                
                <div className='cg-login-modal-button '>
                    <a
                        className={`ui facebook fluid button cg-login-button${ConsentGiven ? "" : " disabled"}`}
                        href={"https://contest-cts.azurewebsites.net/.auth/login/facebook?post_login_redirect_url=" + window.location.pathname + addon}
                    ><Icon name='facebook' /> Login with Facebook</a>
                </div>
                <div className='cg-login-modal-button '>
                    <a
                        className={`ui google plus fluid button cg-login-button${ConsentGiven ? "" : " disabled"}`}
                        href={"https://contest-cts.azurewebsites.net/.auth/login/google?post_login_redirect_url=" + window.location.pathname + addon}
                    ><Icon name='google' /> Login with Google</a>
                </div>
                
            </Modal>
        </Responsive>;
    }

    
    ToggleConsentCheckbox = () =>
    {
        this.setState({
            ConsentGiven: !this.state.ConsentGiven,
        });
    }

}
