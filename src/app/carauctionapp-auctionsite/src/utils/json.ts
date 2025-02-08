import { isDateString } from "./date";

/* eslint-disable  @typescript-eslint/no-explicit-any */
export function jsonReviver(_: string, value: any) {
  if (isDateString(value)) {
    return new Date(value);
  }
  return value;
}
