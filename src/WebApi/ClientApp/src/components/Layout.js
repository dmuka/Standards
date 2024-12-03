import React from 'react';
//import { Container } from 'reactstrap';
import { Outlet } from 'react-router-dom';
import Header from './Header';
import Footer from './Footer';

export default function Layout() {
    return (
            <div>
                <Header />
                <main>
                    <Outlet />
                </main>
                {/*<Container>*/}
                {/*    {this.props.children}*/}
                {/*</Container>*/}
                <Footer />
            </div>
    );
  }