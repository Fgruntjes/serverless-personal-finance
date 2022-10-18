import {jest} from "@jest/globals";
import React from "react";

import {AuthGuardProps} from "./AuthGuard";

jest.mock("./AuthGuard", () => ({children}: AuthGuardProps) => (<>{children}</>));