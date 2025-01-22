import { isDateString } from "./date";

export function jsonReviver(_: string, value: any) {
  if (isDateString(value)) {
    return new Date(value);
  }
  return value;
}
