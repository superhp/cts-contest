import * as React from 'react';
import  { Header }  from './Header';
import  { Footer }  from './Footer';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, {}> {
    public render() {
        return <div >
                    <Header />
                    <div style={{ minHeight: window.innerHeight}}>
                        { this.props.children }
                    </div>
                    <Footer />
            </div>;
    }
}
