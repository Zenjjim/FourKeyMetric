import { Score } from "components/score";
import { COLORS } from "const";
import { ILeadTimeChange } from "types";
import { getHoursfromSeconds } from "utils";
type LeadTimeChangeProps = { data: ILeadTimeChange };
export function LeadTimeChangeScore({ data }: LeadTimeChangeProps) {
  const title = "Lead Time to Change"
  if (!data) return(
    <Score category={"No Data"} color={COLORS.RED} title={title} />
  )
  const [category, color] = ltc_category(data?.medianLeadTimeChange);
  return (
    <Score category={category} color={color} title={title} />
  );
}

export function ltc_category(median: number) {
  median = getHoursfromSeconds(median);
  if (median <= 24) {
    return ["One Day", COLORS.PURPLE];
  } else if (median <= 24 * 7) {
    return ["One Week", COLORS.GREEN];
  } else if (median <= 24 * 7 * 4) {
    return ["One Month", COLORS.YELLOW];
  } else if (median <= 24 * 7 * 4 * 6) {
    return ["Six Months", COLORS.YELLOW];
  } else {
    return ["One Year", COLORS.RED];
  }
}
