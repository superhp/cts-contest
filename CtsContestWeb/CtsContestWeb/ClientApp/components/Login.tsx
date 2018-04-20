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
    constructor(props: any) {
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

        if (window.location.search.indexOf("refresh=true") !== -1) {
            window.location.replace(window.location.origin + window.location.pathname);
        }
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
    public render() {
        let contents = this.props.userInfo.isLoggedIn
            ? this.renderLoggedInView(this.props.userInfo, this.detectIE())
            : Login.renderLoginModal(this.state.modalHeight, this.detectIE(), this.handleResize)

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
                <div className='item cg-responsive-hide'>Hello, {userInfo.name}!</div>
                <div style={{ position: 'relative' }}>
                    <a className='item cg-responsive-hide cg-bold' style={{ height: '100%', width: '100%' }}>My points: {this.props.userInfo.totalBalance}</a>
                </div>
                <a className='item cg-responsive-hide cg-bold' href={"https://cts-contest.azurewebsites.net/.auth/logout?post_logout_redirect_uri=" + window.location.pathname  + addon}>Logout</a>
            </div>
        );
    }

    private static renderLoginModal(height: number, isIE: boolean, handleResize: any) {

        var addon = "";
        if (isIE) {
            addon = "?refresh=true";
        }

        return <a className='item cg-nav-item cg-bold' href={"https://cts-contest.azurewebsites.net/.auth/login/aad?post_login_redirect_url=" + window.location.pathname + addon}>Login</a>;
    }

    
}
