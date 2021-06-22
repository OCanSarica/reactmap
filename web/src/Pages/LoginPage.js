import React from 'react';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { TextField, Button, Icon, Grid } from '@material-ui/core';
import { AccountCircle, Map, Lock } from '@material-ui/icons/';
import { authenticationAction } from '../actions';

class LoginComponent extends React.Component {

    state = {
        username: '',
        password: ''
    };

    constructor(props) {

        super(props);

        this.props.logout();
    }

    render = () => {

        let loginButton = {
            Text: "giriş yap",
            Icon: "check",
            Disabled: false
        };

        if (this.props.loginRequest) {

            loginButton = {
                Text: "giriş yapılıyor...",
                Icon: "hourglass_empty",
                Disabled: true
            };
        }

        return (

            <div className="login">
                <div style={{ paddingLeft: 94, paddingTop: 1, float: "left" }}>
                    <Map style={
                        {
                            color: "#3f51b5",
                            fontSize: 40,
                        }} />
                </div>
                <div style={{ float: "left", paddingLeft: 13 }}>
                    <h2>map app</h2>
                </div>
                <div style={{ paddingLeft: 30, paddingRight: 30, paddingBottom: 10 }}>
                    <Grid
                        container
                        spacing={2}
                        alignItems="flex-end">
                        <Grid item xs={1}>
                            <AccountCircle />
                        </Grid>
                        <Grid item xs={11}>
                            <TextField
                                value={this.state.username}
                                onKeyPress={this.handlePress}
                                onChange={this.handleChange}
                                autoFocus
                                name='username'
                                margin="dense"
                                label="username or e-mail"
                                fullWidth
                            />
                        </Grid>
                    </Grid>
                    <Grid
                        container
                        spacing={2}
                        alignItems="flex-end">
                        <Grid item xs={1}>
                            <Lock />
                        </Grid>
                        <Grid item xs={11}>
                            <TextField
                                value={this.state.password}
                                onKeyPress={this.handlePress}
                                onChange={this.handleChange}
                                name='password'
                                margin="dense"
                                label="password"
                                type="password"
                                fullWidth
                            />
                        </Grid>
                    </Grid>
                </div>
                <div className="login-button">
                    <Button
                        disabled={loginButton.Disabled}
                        type="submit"
                        style={{ width: 200 }}
                        startIcon={<Icon>{loginButton.Icon}</Icon>}
                        variant="contained"
                        onClick={this.login}
                        color="primary">
                        {loginButton.Text}
                    </Button>
                </div>
            </div>
        );
    }

    handleChange = (e) => {

        const { name, value } = e.target;

        this.setState({ [name]: value });
    }

    handlePress = (e) => {

        if (e.key === 'Enter') {

            this.login();
        }
    }

    login = () => {

        console.log(process.env);

        const { username, password } = this.state;

        if (!username || !password) {

            toast.info("kullanıcı adı ve şifre giriniz.");

            return;
        }

        this.props.login(username, password);
    }
}

const mapStateToProps = (state) => {

    const { loginRequest } = state.authenticationReducer;

    return { loginRequest };
}

const mapDispatchToProps = (dispatch) => {

    return {
        login: (user, password) => dispatch(authenticationAction.login(user, password)),
        logout: () => dispatch(authenticationAction.logout())
    }
}

export const LoginPage = connect(mapStateToProps, mapDispatchToProps)(LoginComponent);

export default LoginPage;