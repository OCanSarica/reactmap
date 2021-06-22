import { requestTool } from "../tools";

const login = (username, password) =>
    requestTool.postRequest(window.USER_API_URL + "/api/token",
        {
            "username": username,
            "password": password
        });

export const authenticationService = { login };

export default authenticationService;

