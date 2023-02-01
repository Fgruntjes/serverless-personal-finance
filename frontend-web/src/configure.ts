import {OpenAPI as FunctionIntegrationYnab} from "./generated/App.Function.Integration.Ynab";
import {AuthState} from "./hooks/auth";

export function configure(authState: AuthState | null)
{
    FunctionIntegrationYnab.TOKEN = authState?.token;

    FunctionIntegrationYnab.BASE = process.env.REACT_APP_FUNCTION_INTEGRATION_YNAB_BASE || FunctionIntegrationYnab.BASE;
}