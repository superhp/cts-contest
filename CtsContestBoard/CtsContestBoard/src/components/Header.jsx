import React from 'react';
import { Menu } from 'semantic-ui-react';

export default class Header extends React.Component {
    render() {
        
        return (
            <Menu className="cg-nav" size='large' stackable color='blue' inverted>
                <Menu.Item className='cg-nav-header' header>
                    <img className='cg-nav-logo' src="./img/logo.svg" alt="Cognizant logo" />
                </Menu.Item>
            </Menu>
        )
    }

}