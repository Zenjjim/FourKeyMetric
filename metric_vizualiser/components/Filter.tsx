import { ChevronDownIcon, ChevronUpIcon } from "@chakra-ui/icons";
import {
  Accordion,
  AccordionButton,
  AccordionIcon,
  AccordionItem,
  AccordionPanel,
  Box,
  Button,
  Collapse,
  Flex,
  Text,
  NumberDecrementStepper,
  NumberIncrementStepper,
  NumberInput,
  NumberInputField,
  NumberInputStepper,
  Select,
  Slider,
  SliderFilledTrack,
  SliderThumb,
  SliderTrack,
  useDisclosure,
} from "@chakra-ui/react";
import { COLORS } from "const";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useDebounce } from "use-debounce";
//eslint-disable-next-line @typescript-eslint/no-explicit-any
type FilterProps = { info: any };

export const Filter = ({ info }: FilterProps) => {
  const router = useRouter();

  const [value, setSlider] = useState<number>(Number(router.query?.months));
  const [debounceValue] = useDebounce(value, 1000);
  const handleChange = (value: number) => setSlider(value);
  const { isOpen, onToggle } = useDisclosure();
  const { register, watch, setValue } = useForm({
    mode: "onChange",
    defaultValues: {
      organization: `${router.query?.organization}`,
      project: `${router.query?.project}`,
      repository: `${router.query?.repository}`,
      months: Number(router.query?.months),
    },
  });

  const organizationWatch = watch("organization");
  const projectWatch = watch("project");
  useEffect(() => {
    setSlider(debounceValue);
    setValue("months", debounceValue);
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, [debounceValue]);

  useEffect(() => {
    const subscription = watch((value) => {
      const organization = value.organization;
      const project = organization ? value.project : "";
      const repository = project ? value.repository : "";
      router.push({
        pathname: "/",
        query: {
          ...router.query,
          months: value.months,
          organization: organization,
          project: project,
          repository: repository,
        },
      });
    });
    return () => subscription.unsubscribe();
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, [watch]);

  return (
    <>

      <Accordion allowToggle background={COLORS.PAPER}
        borderRadius={"10px"}
        display="flex"
        flexDirection="column"
        gap="10px"
        marginBottom="20px"
        padding="10px"
        width="100%">
        <AccordionItem style={{ borderWidth: "0" }}>
          <AccordionButton>
            <Box flex='1' textAlign='left'>
              <Text fontSize={"2xl"}>
                Filters
              </Text>
            </Box>
            <AccordionIcon />
          </AccordionButton>
          <AccordionPanel pb={4}>
            <form style={{ display: "flex", gap: "20px" }}>
              <Select
                placeholder={"Organization"}
                {...register("organization")}
                color="white"
                onChange={(e) => {
                  setValue("organization", e.target.value);
                  setValue("project", ``);
                  setValue("repository", ``);
                }}
              >
                {info && Object.keys(info).map((d: string) => (
                  <option key={d} value={d}>
                    {d}
                  </option>
                ))}
              </Select>
              <Select
                disabled={!organizationWatch}
                placeholder={"Project"}
                {...register("project")}
                onChange={(e) => {
                  setValue("project", e.target.value);
                  setValue("repository", ``);
                }}
              >
                {info && Object.keys(info[organizationWatch] ?? "").map((d: string) => (
                  <option key={d} value={d}>
                    {d}
                  </option>
                ))}
              </Select>
              <Select
                disabled={!projectWatch || !organizationWatch}
                placeholder={"Repository"}
                {...register("repository")}
              >
                {info && ((info[organizationWatch] ?? "")[projectWatch] ?? []).map(
                  (d: string) => (
                    <option key={d} value={d}>
                      {d}
                    </option>
                  )
                )}
              </Select>
              <div style={{ position: "relative", width: "100%" }} >
                <div style={{ position: "absolute", top: "-30px" }}>Selected Months</div>
                <Flex width="100%">
                  <NumberInput
                    min={1}
                    maxW="100px"
                    mr="2rem"
                    onChange={(_: string, value: number) => handleChange(value)}
                    value={value}
                  >
                    <NumberInputField />
                    <NumberInputStepper>
                      <NumberIncrementStepper />
                      <NumberDecrementStepper />
                    </NumberInputStepper>
                  </NumberInput>
                  <div style={{ width: "100%", marginRight: "30px" }}>
                    <Slider
                      flex="1"
                      focusThumbOnChange={false}
                      onChange={handleChange}
                      value={value}
                      min={1}
                      max={24}
                    >
                      <SliderTrack>
                        <SliderFilledTrack />
                      </SliderTrack>

                      <SliderThumb boxSize="32px" fontSize="sm">
                        value
                      </SliderThumb>
                    </Slider>
                  </div>

                </Flex>
              </div>

            </form>
          </AccordionPanel>
        </AccordionItem>
      </Accordion>
    </>
  );
};
