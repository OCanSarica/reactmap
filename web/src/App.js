import React from 'react';
import { Router, Route, Switch, Redirect } from 'react-router-dom';
import { connect } from 'react-redux';
import { ToastContainer } from 'react-toastify';
import { createMuiTheme, MuiThemeProvider } from '@material-ui/core/styles/';
import 'semantic-ui-css/semantic.min.css'
import { PrivateRoute } from './components';
import { LoginPage, HomePage } from "./pages";
import { history } from "./tools";

class App extends React.Component {

    render = () => {

        return (
            <MuiThemeProvider theme={theme}>
                <div className="container">
                    <ToastContainer />
                    <Router history={history}>
                        <Switch>
                            <PrivateRoute exact path="/" component={HomePage} />
                            <Route path="/login" component={LoginPage} />
                            <Redirect from="*" to="/" />
                        </Switch>
                    </Router>
                </div>
            </MuiThemeProvider>
        );
    }
}

const theme = createMuiTheme({
    palette: {
        secondary: {
            main: '#ffc800',
        },
    },
    typography: {
        fontSize: 15,
    },
});

const mapStateToProps = (state) => {

    const { user } = state.authenticationReducer;

    return { user };
}

export default connect(mapStateToProps, null)(App);
