import * as Plot from "@observablehq/plot";
import { useEffect, useRef } from "react";
import { ITimeToRestoreService } from "types";
import { getDateOfWeek } from "utils";
type TimeToRestoreServiceProps = {
  data?: ITimeToRestoreService;
  months: number;
};

export function TimeToRestoreService({
  data,
  months,
}: TimeToRestoreServiceProps) {
  const headerRef = useRef(null);

  useEffect(() => {
    if (data === undefined) {
      return;
    }

    const getmontlyTransformedDataMedian = (data: ITimeToRestoreService) =>
      data.monthlyRestoreServiceTime.map((d) => ({
        date: new Date(d.key.yearNumber, d.key.monthNumber),
        median: d.median / 3600,
      }));
    const getweeklyTransformedDataMedian = (data: ITimeToRestoreService) =>
      data.weeklyRestoreServiceTime.map((d) => ({
        date: getDateOfWeek(d.key.weekNumber, d.key.yearNumber),
        median: d.median / 3600,
      }));

    const transformedDataMedian =
      months < 2
        ? getmontlyTransformedDataMedian(data)
        : getweeklyTransformedDataMedian(data);

    const chart = Plot.plot({
      style: {
        background: "transparent",
      },
      y: {
        grid: true,
        label: "Hours",
      },
      color: {
        type: "diverging",
        scheme: "burd",
        range: ["red", "blue"],
        interpolate: "hcl",
      },
      marks: [
        Plot.ruleY([0]),
        Plot.line(transformedDataMedian, {
          x: "date",
          y: "median",
          curve: "basis",
          marker: "circle",
          stroke: "steelblue",
          opacity: 1,
        }),
      ],
    });
    // @ts-ignore
    headerRef.current.append(chart);
    return () => chart.remove();
  }, [data, months]);

  return <div ref={headerRef}></div>;
}
