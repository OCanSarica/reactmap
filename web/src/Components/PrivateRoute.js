import React from 'react';
import { Route, Redirect } from 'react-router-dom';

export const PrivateRoute = ({ component: Component, ...rest }) => (

    <Route {...rest} render={props => (

        localStorage.getItem('user_') ?
            <Component {...props} /> :
            <Redirect to={{ pathname: '/login', state: { from: props.location } }} />
    )} />
)

export default PrivateRoute;