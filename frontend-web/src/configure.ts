import {OpenAPI as FunctionIntegrationYnab} from "./generated/App.Function.Integration.Ynab";
import {TokenFetcher} from "./hooks/auth";

export function configure(tokenFetcher: TokenFetcher)
{
    FunctionIntegrationYnab.TOKEN = async () => (await tokenFetcher()) || ""
    FunctionIntegrationYnab.BASE = process.env.REACT_APP_FUNCTION_INTEGRATION_YNAB_BASE || FunctionIntegrationYnab.BASE;
}