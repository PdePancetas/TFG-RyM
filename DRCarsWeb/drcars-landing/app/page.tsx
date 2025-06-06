"use client"

import { useBreakpoint } from "@/hooks/use-breakpoint"
import { DesktopView } from "@/components/desktop-view"
import { MobileView } from "@/components/mobile-view"

export default function Home() {
  const isDesktop = useBreakpoint("md")

  return <>{isDesktop ? <DesktopView /> : <MobileView />}</>
}
